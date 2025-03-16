package csc13001.plantpos.application.dtos.order;

import csc13001.plantpos.domain.enums.OrderStatus;
import csc13001.plantpos.utils.http.JsonModel;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class UpdateOrderStatusDTO extends JsonModel {
    OrderStatus status;
}
