package csc13001.plantpos.statistic;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.product.dtos.ProductStatisticsDTO;
import csc13001.plantpos.statistic.dtos.ProductsStatisticsDTO;
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;
import csc13001.plantpos.statistic.dtos.StatisticsRequestDTO;

@RestController
@RequestMapping("api/statistics")
public class StatisticsController {
    @Autowired
    private StatisticsService statisticsService;

    @GetMapping("/sales")
    public ResponseEntity<?> getMethodName(
            @RequestBody StatisticsRequestDTO statisticsRequestDTO,
            BindingResult bindingResult) {
        if (bindingResult.hasErrors()) {
            return HttpResponse.badRequest("Dữ liệu yêu cầu không hợp lệ");
        }

        SalesStatisticsDTO salesStatistics = statisticsService.getSalesStatistics(
                statisticsRequestDTO.getTimeType(),
                statisticsRequestDTO.getStartDate(),
                statisticsRequestDTO.getEndDate());
        return HttpResponse.ok("Lấy thống kê doanh số thành công", salesStatistics);
    }

    @GetMapping("/products-review")
    public ResponseEntity<?> getProductStatisticsReview() {
        ProductsStatisticsDTO productStatistics = statisticsService.getProductStatisticsReview();
        return HttpResponse.ok("Lấy thống kê sản phẩm thành công", productStatistics);
    }

    @GetMapping("/products")
    public ResponseEntity<?> getProductStatistics() {
        List<ProductStatisticsDTO> productStatistics = statisticsService.topSellingProducts(null);
        return HttpResponse.ok("Lấy thống kê sản phẩm thành công", productStatistics);
    }
}
