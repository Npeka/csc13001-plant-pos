package csc13001.plantpos.statistic;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import csc13001.plantpos.utils.http.HttpResponse;
import csc13001.plantpos.product.dtos.ProductDTO;
import csc13001.plantpos.statistic.dtos.ProductsStatisticsDTO;
import csc13001.plantpos.statistic.dtos.SalesStatisticsDTO;

@RestController
@RequestMapping("api/statistics")
public class StatisticsController {
    @Autowired
    private StatisticsService statisticsService;

    @GetMapping("/sales")
    public ResponseEntity<?> getMethodName(
            @RequestParam TimeType timeType,
            @RequestParam LocalDateTime startDate,
            @RequestParam LocalDateTime endDate) {
        SalesStatisticsDTO salesStatistics = statisticsService
                .getSalesStatistics(timeType, startDate, endDate);
        return HttpResponse.ok("Lấy thống kê doanh số thành công", salesStatistics);
    }

    @GetMapping("/products-review")
    public ResponseEntity<?> getProductStatisticsReview() {
        ProductsStatisticsDTO productStatistics = statisticsService.getProductStatisticsReview();
        return HttpResponse.ok("Lấy thống kê sản phẩm thành công", productStatistics);
    }

    @GetMapping("/products")
    public ResponseEntity<?> getProductStatistics() {
        List<ProductDTO> productStatistics = statisticsService.topSellingProducts(null);
        return HttpResponse.ok("Lấy thống kê sản phẩm thành công", productStatistics);
    }
}
