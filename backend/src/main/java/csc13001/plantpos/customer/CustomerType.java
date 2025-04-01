package csc13001.plantpos.customer;

public enum CustomerType {
    All(0, "All"),
    None(1, "None"),
    Bronze(2, "Bronze"),
    Silver(3, "Silver"),
    Gold(4, "Gold"),
    Platinum(5, "Platinum");

    private final int rank;
    private final String name;

    CustomerType(int rank, String name) {
        this.rank = rank;
        this.name = name;
    }

    public int getRank() {
        return rank;
    }

    public String getName() {
        return name;
    }

    public boolean isHigherThanOrEqualTo(CustomerType other) {
        return this.rank >= other.rank;
    }

    public boolean isLowerThanOrEqualTo(CustomerType other) {
        return this.rank <= other.rank;
    }

    public boolean isHigherThan(CustomerType other) {
        return this.rank > other.rank;
    }

    public boolean isLowerThan(CustomerType other) {
        return this.rank < other.rank;
    }

    @Override
    public String toString() {
        return this.name;
    }
}
