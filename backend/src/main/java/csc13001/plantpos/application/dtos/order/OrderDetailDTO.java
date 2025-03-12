package csc13001.plantpos.application.dtos.order;

import csc13001.plantpos.utils.http.JsonModel;
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
    private int quantity;
    private String note;
}
