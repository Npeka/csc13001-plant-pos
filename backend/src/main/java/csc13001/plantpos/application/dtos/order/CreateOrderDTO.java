package csc13001.plantpos.application.dtos.order;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonInclude;
import csc13001.plantpos.domain.enums.OrderStatus;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class CreateOrderDTO {
    private Long customer_id;
    private Long staff_id;
    private List<OrderDetailDTO> items;
    private BigDecimal total_price;

    @Enumerated(EnumType.STRING)
    private OrderStatus status;
}
