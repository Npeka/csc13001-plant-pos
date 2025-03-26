package csc13001.plantpos;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.data.jpa.repository.config.EnableJpaAuditing;

@SpringBootApplication(scanBasePackages = "csc13001.plantpos")
@EnableJpaAuditing
public class PlantposApplication {
    public static void main(String[] args) {
        SpringApplication.run(PlantposApplication.class, args);
    }
}