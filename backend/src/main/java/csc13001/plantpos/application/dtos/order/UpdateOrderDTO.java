package csc13001.plantpos.application.dtos.order;

import csc13001.plantpos.domain.enums.OrderStatus;
import csc13001.plantpos.utils.http.JsonModel;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@EqualsAndHashCode(callSuper = true)
public class UpdateOrderDTO extends JsonModel {
    private Long id;
    @Enumerated(EnumType.STRING)
    private OrderStatus status;
}
