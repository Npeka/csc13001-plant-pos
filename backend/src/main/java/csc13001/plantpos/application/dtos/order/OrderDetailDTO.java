package csc13001.plantpos.application.dtos.order;

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
public class OrderDetailDTO extends JsonModel {
    private Long productId;

    private Long discountId;

    @Min(value = 1, message = "Quantity must be greater than 0")
    private int quantity;

    private String note;
}
