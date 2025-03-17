package csc13001.plantpos.domain.models;

import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
public class Transaction {
    private String reference;
    private int amount;
    private String accountNumber;
    private String description;
    private String transactionDateTime;
    private String virtualAccountName;
    private String virtualAccountNumber;
    private String counterAccountBankId;
    private String counterAccountBankName;
    private String counterAccountName;
    private String counterAccountNumber;
}
