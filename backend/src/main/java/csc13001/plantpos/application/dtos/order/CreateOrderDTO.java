package csc13001.plantpos.application.dtos.order;

import csc13001.plantpos.domain.enums.OrderStatus;
import csc13001.plantpos.utils.http.JsonModel;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class CreateOrderDTO extends JsonModel {
    private String customerPhone;

    private Long staffId;
    private List<OrderDetailDTO> items;
    private BigDecimal totalPrice;

    @Enumerated(EnumType.STRING)
    private OrderStatus status;
}
