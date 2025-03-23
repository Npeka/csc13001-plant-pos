package csc13001.plantpos.order.dtos;

import csc13001.plantpos.utils.http.JsonModel;
import jakarta.validation.Valid;
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

    private BigDecimal totalPrice;

    private Long discountId;

    @Valid
    private List<OrderItemlDTO> items;
}
