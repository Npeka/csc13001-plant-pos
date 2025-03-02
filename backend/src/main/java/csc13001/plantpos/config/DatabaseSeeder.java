package csc13001.plantpos.config;

import csc13001.plantpos.domain.models.Category;
import csc13001.plantpos.domain.models.Product;
import csc13001.plantpos.domain.models.User;
import csc13001.plantpos.adapters.repositories.CategoryRepository;
import csc13001.plantpos.adapters.repositories.ProductRepository;
import csc13001.plantpos.adapters.repositories.UserRepository;
import csc13001.plantpos.application.dtos.auth.RegisterDTO;
import csc13001.plantpos.application.services.AuthService;

import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.autoconfigure.kafka.KafkaProperties.Producer;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;

import java.math.BigDecimal;
import java.util.HashSet;
import java.util.List;

@Configuration
public class DatabaseSeeder {

    private final BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();

    @Bean
    CommandLineRunner seedDatabase(
            UserRepository userRepository,
            CategoryRepository categoryRepo,
            ProductRepository productRepo) {
        return _ -> {
            // Seed users
            if (userRepository.count() == 0) {
                List<User> users = List.of(
                        new User("admin", bCryptPasswordEncoder.encode("admin"), null, true),
                        new User("user", bCryptPasswordEncoder.encode("user")));
                userRepository.saveAll(users);
            }

            // Seed categories
            if (categoryRepo.count() == 0) {
                List<Category> categories = List.of(
                        new Category("Succulents", "Plants that store water in their leaves."),
                        new Category("Indoor Plants", "Plants that thrive indoors."),
                        new Category("Outdoor Plants", "Plants that thrive outdoors."));
                categoryRepo.saveAll(categories);
            }

            // Seed products
            if (productRepo.count() == 0) {
                // List<Product> products = List.of(
                // new Product("Aloe Vera", "A low-maintenance succulent plant.", new
                // BigDecimal("15.99"), 50, 2,
                // "Indoor", "Medium", 3, 7, new
                // HashSet<>(List.of(categoryRepo.findById(1L).get()))),

                // new Product("Snake Plant", "A hardy indoor plant.", new BigDecimal("19.99"),
                // 30, 1, "Indoor",
                // "Medium", 2, 7, new HashSet<>(List.of(categoryRepo.findById(1L).get()))),

                // new Product("Spider Plant", "A popular indoor plant.", new
                // BigDecimal("14.99"), 40, 2, "Indoor",
                // "Medium", 3, 7, new HashSet<>(List.of(categoryRepo.findById(1L).get()))));
                // productRepo.saveAll(products);
            }
        };
    }

}
