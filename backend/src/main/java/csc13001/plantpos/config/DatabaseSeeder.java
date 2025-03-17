package csc13001.plantpos.config;

import csc13001.plantpos.adapters.repositories.CategoryRepository;
import csc13001.plantpos.adapters.repositories.InventoryRepository;
import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.adapters.repositories.UserRepository;
import csc13001.plantpos.application.services.InventoryService;
import csc13001.plantpos.application.services.MinIOService;
import csc13001.plantpos.domain.enums.MinioBucket;
import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.domain.models.Inventory;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.domain.models.User;
import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.mock.web.MockMultipartFile;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.web.multipart.MultipartFile;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.math.BigDecimal;
import java.util.Date;
import java.util.List;
import java.util.Map;

@Configuration
public class DatabaseSeeder {

    private final BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();

    @Bean
    CommandLineRunner seedDatabase(
            UserRepository userRepository,
            CategoryRepository categoryRepo,
            ProductRepository productRepo,
            InventoryRepository inventoryRepo,
            InventoryService inventoryService,
            MinIOService minioService) {
        return _ -> {
            // Seed users
            if (userRepository.count() == 0) {
                List<User> users = List.of(
                        // admin
                        new User("Nguyen Phuc Khang", "admin",
                                bCryptPasswordEncoder.encode("admin"), true),
                        new User("Nguyen Trong Nhan", "admin1",
                                bCryptPasswordEncoder.encode("admin1"), true),
                        new User("Nguyen Minh Toan", "admin2",
                                bCryptPasswordEncoder.encode("admin2"), true),
                        // staff
                        new User("Nguyen Phuc Khang", "staff",
                                bCryptPasswordEncoder.encode("staff")),
                        new User("Nguyen Trong Nhan", "staff1",
                                bCryptPasswordEncoder.encode("staff1")),
                        new User("Nguyen Minh Toan", "staff2",
                                bCryptPasswordEncoder.encode("staff2")));
                userRepository.saveAll(users);
            }

            // Seed categories
            if (categoryRepo.count() == 0) {
                List<Category> categories = List.of(
                        new Category("Low Light Plants",
                                "Plants that require little sunlight."),
                        new Category("Miniature Plants",
                                "Small decorative plants for desks and shelves."),
                        new Category("Foliage Plants",
                                "Ornamental plants with beautiful leaves."),
                        new Category("Aquatic Plants",
                                "Plants that grow in water environments."));
                categoryRepo.saveAll(categories);
            }

            // Seed products
            if (productRepo.count() == 0) {
                List<Product> products = List.of(
                        new Product("Lucky Bamboo 5 Stalks",
                                "A lucky plant with five stalks.",
                                null,
                                new BigDecimal("750000"), 0, 3,
                                "Indoor", "Low", 4, 10,
                                categoryRepo.findByName("Low Light Plants").get()),

                        new Product("Money Tree Three Stems",
                                "A money tree with three stems, perfect for desks.",
                                null,
                                new BigDecimal("280000"), 0, 2,
                                "Indoor", "Low", 3, 9,
                                categoryRepo.findByName("Low Light Plants").get()),

                        new Product("Imperial Green Philodendron",
                                "An elegant green philodendron for low-light environments.",
                                null,
                                new BigDecimal("120000"),
                                0, 2,
                                "Indoor", "Low", 3, 8,
                                categoryRepo.findByName("Low Light Plants").get()),

                        new Product("Spider Plant in Smiley Ceramic Pot SPI004",
                                "The Spider Plant (also known as Chlorophytum) is a highly resilient plant that thrives in Vietnam's hot climate.",
                                null,
                                new BigDecimal("120000"), 0, 3,
                                "Indoor", "Low", 4, 10,
                                categoryRepo.findByName("Low Light Plants").get()),

                        new Product("Heart-Shaped Lucky Plant",
                                "A heart-shaped lucky plant, ideal for gifts.",
                                null,
                                new BigDecimal("240000"), 0, 2,
                                "Indoor", "Mini", 3, 7,
                                categoryRepo.findByName("Miniature Plants").get()),

                        new Product("Mini Areca Palm",
                                "A small areca palm in a cute cat-themed pot.",
                                null,
                                new BigDecimal("250000"), 0, 3,
                                "Indoor", "Mini", 4, 9,
                                categoryRepo.findByName("Miniature Plants").get()),

                        new Product("Braided Money Tree",
                                "A braided money tree with a decorative ceramic pot.",
                                null,
                                new BigDecimal("250000"), 0, 2,
                                "Indoor", "Mini", 3, 8,
                                categoryRepo.findByName("Miniature Plants").get()),

                        new Product("Red Aglaonema",
                                "A beautiful Aglaonema with red-tinted leaves.",
                                null,
                                new BigDecimal("320000"), 0, 3,
                                "Indoor", "Foliage", 4, 9,
                                categoryRepo.findByName("Foliage Plants").get()),

                        new Product("Golden Philodendron",
                                "A golden philodendron variety with vibrant foliage.",
                                null,
                                new BigDecimal("180000"), 0, 2,
                                "Indoor", "Foliage", 3, 8,
                                categoryRepo.findByName("Foliage Plants").get()),

                        new Product("Philodendron Xanadu",
                                "A unique philodendron variety with lush green leaves.",
                                null,
                                new BigDecimal("950000"), 0, 4,
                                "Indoor", "Foliage", 5, 10,
                                categoryRepo.findByName("Foliage Plants").get()),

                        new Product("Pink Anthurium",
                                "A striking pink anthurium with an orange stem.",
                                null,
                                new BigDecimal("550000"), 0, 3,
                                "Indoor", "Foliage", 4, 9,
                                categoryRepo.findByName("Foliage Plants").get()),

                        new Product("Golden-Edged Lucky Bamboo",
                                "A water-grown lucky plant with golden-edged leaves.",
                                null,
                                new BigDecimal("220000"), 0, 3,
                                "Indoor", "Aquatic", 4, 9,
                                categoryRepo.findByName("Aquatic Plants").get()),

                        new Product("Mini Snake Plant",
                                "A mini snake plant with dark green and gold accents.",
                                null,
                                new BigDecimal("220000"), 0, 2,
                                "Indoor", "Aquatic", 3, 8,
                                categoryRepo.findByName("Aquatic Plants").get()),

                        new Product("Red Star Aglaonema",
                                "A vibrant red Aglaonema variety for water environments.",
                                null,
                                new BigDecimal("240000"), 0, 3,
                                "Indoor", "Aquatic", 4, 9,
                                categoryRepo.findByName("Aquatic Plants").get()),

                        new Product("Apple Green Calathea",
                                "A beautiful Calathea with apple-green leaves.",
                                null,
                                new BigDecimal("240000"), 0, 3,
                                "Indoor", "Aquatic", 4, 9,
                                categoryRepo.findByName("Aquatic Plants").get()));

                productRepo.saveAll(products);
            }

            if (productRepo.findAll().get(0).getImageUrl() == null) {
                String basePath = "./src/main/java/csc13001/plantpos/config/ProductImages/";
                Map<String, String> imageMapping = Map.of(
                        "Lucky Bamboo 5 Stalks", "Lucky Bamboo 5 Stalks.jpg",
                        "Money Tree Three Stems", "Money Tree Three Stems.jpg",
                        "Imperial Green Philodendron", "Imperial Green Philodendron.jpg",
                        "Spider Plant in Smiley Ceramic Pot SPI004",
                        "Spider Plant in Smiley Ceramic Pot SPI004.jpg");

                for (String productName : imageMapping.keySet()) {
                    String imagePath = basePath + imageMapping.get(productName);
                    File imageFile = new File(imagePath);

                    if (imageFile.exists()) {
                        try (FileInputStream fileInputStream = new FileInputStream(imageFile)) {
                            MultipartFile multipartFile = new MockMultipartFile(
                                    imageFile.getName(),
                                    imageFile.getName(),
                                    "image/jpeg",
                                    fileInputStream);

                            String imageUrl = minioService.uploadFile(multipartFile,
                                    MinioBucket.PRODUCT);

                            Product product = productRepo.findByName(productName)
                                    .orElse(null);
                            if (product != null) {
                                product.setImageUrl(imageUrl);
                                productRepo.save(product);
                            }
                        } catch (IOException e) {
                            System.err.println("Error reading image file: " + imagePath);
                        }
                    } else {
                        System.err.println("Error image file not found: " + imagePath);
                    }
                }
            }

            if (inventoryRepo.count() == 0) {
                List<Product> products = productRepo.findAll();
                for (Product product : products) {
                    Inventory inventory = Inventory.builder()
                            .supplier("Default Supplier")
                            .product(product)
                            .quantity(100)
                            .purchasePrice(product.getPrice()
                                    .multiply(new BigDecimal("0.8")))
                            .purchaseDate(new Date())
                            .build();
                    inventoryService.createInventoryItem(inventory);
                }
            }

        };
    }

}
