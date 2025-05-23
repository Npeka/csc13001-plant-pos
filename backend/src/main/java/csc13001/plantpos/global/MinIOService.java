package csc13001.plantpos.global;

import csc13001.plantpos.global.enums.MinioBucket;
import csc13001.plantpos.product.exception.ProductException;
import io.minio.BucketExistsArgs;
import io.minio.MakeBucketArgs;
import io.minio.MinioClient;
import io.minio.PutObjectArgs;
import io.minio.SetBucketPolicyArgs;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.io.InputStream;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class MinIOService {
    private final MinioClient minioClient;

    @Value("${minio.url}")
    private String minioUrl;

    @Value("${minio.bucket.product}")
    private String productBucket;

    @Value("${minio.bucket.staff}")
    private String staffBucket;

    private void ensureBucketExists(String bucketName) {
        try {
            boolean found = minioClient.bucketExists(BucketExistsArgs.builder().bucket(bucketName).build());
            if (!found) {
                minioClient.makeBucket(MakeBucketArgs.builder().bucket(bucketName).build());
            }

            String policy = """
                    {
                      "Version": "2012-10-17",
                      "Statement": [
                        {
                          "Effect": "Allow",
                          "Principal": "*",
                          "Action": ["s3:GetObject"],
                          "Resource": ["arn:aws:s3:::%s/*"]
                        }
                      ]
                    }
                    """.formatted(bucketName);
            minioClient.setBucketPolicy(
                    SetBucketPolicyArgs.builder().bucket(bucketName).config(policy).build());
        } catch (Exception e) {
            throw new RuntimeException("Lỗi kiểm tra/khởi tạo bucket MinIO: " + bucketName, e);
        }
    }

    @Transactional(propagation = Propagation.REQUIRES_NEW)
    public String uploadFile(MultipartFile file, MinioBucket minioBucket) {
        validateImage(file);

        try {
            String bucketName;

            switch (minioBucket) {
                case PRODUCT:
                    bucketName = productBucket;
                    break;
                case STAFF:
                    bucketName = staffBucket;
                    break;
                default:
                    throw new IllegalArgumentException("Loại ảnh không hợp lệ!");
            }

            ensureBucketExists(bucketName);

            String fileExtension = getFileExtension(file.getOriginalFilename());
            String fileName = UUID.randomUUID() + fileExtension;
            InputStream inputStream = file.getInputStream();

            minioClient.putObject(
                    PutObjectArgs.builder()
                            .bucket(bucketName)
                            .object(fileName)
                            .stream(inputStream, file.getSize(), -1)
                            .contentType(file.getContentType())
                            .build());

            return minioUrl + "/" + bucketName + "/" + fileName;

        } catch (Exception e) {
            throw new RuntimeException("Lỗi khi upload file lên MinIO", e);
        }
    }

    private String getFileExtension(String fileName) {
        if (fileName != null && fileName.contains(".")) {
            return fileName.substring(fileName.lastIndexOf("."));
        }
        return "";
    }

    public void deleteFile(String fileUrl) {
        try {
            String relativePath = fileUrl.replace(minioUrl + "/", "");
            int firstSlashIndex = relativePath.indexOf("/");
            if (firstSlashIndex == -1) {
                throw new IllegalArgumentException("File URL không hợp lệ: " + fileUrl);
            }

            String bucketName = relativePath.substring(0, firstSlashIndex);
            String objectName = relativePath.substring(firstSlashIndex + 1);

            minioClient.removeObject(
                    io.minio.RemoveObjectArgs.builder()
                            .bucket(bucketName)
                            .object(objectName)
                            .build());

        } catch (Exception e) {
            throw new RuntimeException("Lỗi khi xóa file trên MinIO: " + fileUrl, e);
        }
    }

    private void validateImage(MultipartFile image) {
        if (image == null) {
            return;
        }

        String contentType = image.getContentType();
        if (contentType == null || !(contentType.startsWith("image/"))) {
            throw new ProductException.ProductWrongTypeImageException();
        }
    }
}
