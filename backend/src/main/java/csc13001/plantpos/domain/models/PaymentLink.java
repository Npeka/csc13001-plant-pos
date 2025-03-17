package csc13001.plantpos.domain.models;

import java.sql.Timestamp;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
public class PaymentLink {
    private Long id;
    private Long orderId;
    private String paymentLinkId;
    private String checkoutUrl;
    private String status;
    private Timestamp createdAt;
}
