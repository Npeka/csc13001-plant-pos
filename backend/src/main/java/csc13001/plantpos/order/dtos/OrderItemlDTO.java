package csc13001.plantpos.order.dtos;

import csc13001.plantpos.utils.http.JsonModel;
import jakarta.validation.constraints.Min;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class OrderItemlDTO extends JsonModel {
    private Long productId;

    @Min(value = 1, message = "Số lượng phải lớn hơn 0")
    private int quantity;
}
