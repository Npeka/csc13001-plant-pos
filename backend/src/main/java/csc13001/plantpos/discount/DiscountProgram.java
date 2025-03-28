package csc13001.plantpos.discount;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonInclude;

import csc13001.plantpos.customer.CustomerType;
import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.Date;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
@JsonInclude(JsonInclude.Include.NON_NULL)
@Entity
@Table(name = "discount_programs")
public class DiscountProgram {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "discount_id")
    private Long discountId;

    @NotBlank(message = "Name is mandatory")
    @Column(name = "name")
    private String name;

    @NotNull(message = "Discount rate is mandatory")
    @Column(name = "discount_rate")
    private Double discountRate;

    @NotNull(message = "Start date is mandatory")
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    @Column(name = "start_date")
    private Date startDate;

    @NotNull(message = "End date is mandatory")
    @JsonFormat(shape = JsonFormat.Shape.STRING, pattern = "yyyy-MM-dd")
    @Column(name = "end_date")
    private Date endDate;

    @Column(name = "applicable_customer_type")
    private CustomerType applicableCustomerType;

    public boolean isActive() {
        Date now = new Date();
        return now.after(startDate) && now.before(endDate);
    }

    public boolean isApplicableToCustomerType(CustomerType customerType) {
        return applicableCustomerType == null || applicableCustomerType == customerType;
    }
}