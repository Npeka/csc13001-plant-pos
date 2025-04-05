package csc13001.plantpos.config;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;

import csc13001.plantpos.category.Category;
import csc13001.plantpos.category.CategoryRepository;
import csc13001.plantpos.customer.Customer;
import csc13001.plantpos.customer.CustomerRepository;
import csc13001.plantpos.discount.DiscountProgram;
import csc13001.plantpos.discount.DiscountProgramRepository;
import csc13001.plantpos.global.MinIOService;
import csc13001.plantpos.inventory.InventoryRepository;
import csc13001.plantpos.inventory.InventoryService;
import csc13001.plantpos.inventory.dtos.InventoryDTO;
import csc13001.plantpos.notification.NotificationRepository;
import csc13001.plantpos.notification.NotificationService;
import csc13001.plantpos.notification.dtos.CreateNotificationDTO;
import csc13001.plantpos.order.OrderRepository;
import csc13001.plantpos.order.OrderService;
import csc13001.plantpos.order.dtos.CreateOrderDTO;
import csc13001.plantpos.product.Product;
import csc13001.plantpos.product.ProductRepository;
import csc13001.plantpos.user.User;
import csc13001.plantpos.user.UserRepository;
import io.jsonwebtoken.io.IOException;
import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.ClassPathResource;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;

import java.io.InputStream;
import java.util.List;

@Configuration
public class DatabaseSeeder {

    private final BCryptPasswordEncoder bCryptPasswordEncoder = new BCryptPasswordEncoder();

    @Bean
    CommandLineRunner seedDatabase(
            UserRepository userRepository,
            CustomerRepository customerRepository,
            CategoryRepository categoryRepository,
            ProductRepository productRepository,
            DiscountProgramRepository discountProgramRepository,
            InventoryRepository inventoryRepository,
            InventoryService inventoryService,
            OrderRepository orderRepository,
            OrderService orderService,
            MinIOService minioService,
            NotificationRepository notificationRepository,
            NotificationService notificationService) {
        return _ -> {
            String basePathSeedData = "SeedData/";
            ObjectMapper objectMapper = new ObjectMapper();
            objectMapper.registerModule(new JavaTimeModule());
            // Create user accounts
            if (userRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "users.json")
                        .getInputStream()) {
                    List<User> users = objectMapper.readValue(inputStream, new TypeReference<List<User>>() {
                    });
                    users.forEach(user -> user.setPassword(bCryptPasswordEncoder.encode(user.getPassword())));
                    userRepository.saveAll(users);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            // Create customers
            if (customerRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "customers.json")
                        .getInputStream()) {
                    List<Customer> customers = objectMapper.readValue(inputStream, new TypeReference<List<Customer>>() {
                    });
                    customerRepository.saveAll(customers);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            // Create categories
            if (categoryRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "categories.json")
                        .getInputStream()) {
                    List<Category> categories = objectMapper.readValue(inputStream,
                            new TypeReference<List<Category>>() {
                            });
                    categoryRepository.saveAll(categories);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            // Create products
            if (productRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "products.json")
                        .getInputStream()) {
                    List<Product> products = objectMapper.readValue(inputStream, new TypeReference<List<Product>>() {
                    });
                    productRepository.saveAll(products);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            if (discountProgramRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "discounts.json")
                        .getInputStream()) {
                    List<DiscountProgram> discountPrograms = objectMapper.readValue(inputStream,
                            new TypeReference<List<DiscountProgram>>() {
                            });
                    discountProgramRepository.saveAll(discountPrograms);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            if (inventoryRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "inventories.json")
                        .getInputStream()) {
                    List<InventoryDTO> inventoriesDTO = objectMapper.readValue(inputStream,
                            new TypeReference<List<InventoryDTO>>() {
                            });
                    for (InventoryDTO inventoryDTO : inventoriesDTO) {
                        inventoryService.createInventory(inventoryDTO);
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            if (orderRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "orders.json")
                        .getInputStream()) {
                    List<CreateOrderDTO> orders = objectMapper.readValue(inputStream,
                            new TypeReference<List<CreateOrderDTO>>() {
                            });
                    for (CreateOrderDTO order : orders) {
                        orderService.createOrder(order);
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            if (notificationRepository.count() == 0) {
                try (InputStream inputStream = new ClassPathResource(basePathSeedData + "notifications.json")
                        .getInputStream()) {

                    List<CreateNotificationDTO> notifications = objectMapper
                            .readValue(inputStream, new TypeReference<List<CreateNotificationDTO>>() {
                            });

                    for (CreateNotificationDTO notification : notifications) {
                        notificationService.createNotification(notification);
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        };
    }
}
