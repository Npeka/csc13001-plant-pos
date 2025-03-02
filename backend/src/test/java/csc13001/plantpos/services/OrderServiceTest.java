package csc13001.plantpos.services;

import csc13001.plantpos.application.dtos.order.CreateOrderDTO;
import csc13001.plantpos.application.services.OrderService;
import csc13001.plantpos.domain.models.Order;
import csc13001.plantpos.adapters.repositories.OrderRepository;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.MockitoJUnitRunner;
import org.springframework.boot.test.context.SpringBootTest;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;

@SpringBootTest
@RunWith(MockitoJUnitRunner.class)
public class OrderServiceTest {

    @Mock
    private OrderRepository orderRepository;

    @InjectMocks
    private OrderService orderService;

    @Test
    public void testCreateOrder() {
        Order order = new Order();
        Mockito.when(orderRepository.save(Mockito.any(Order.class))).thenReturn(order);
        CreateOrderDTO createOrderDTO = new CreateOrderDTO();
        Order result = orderService.createOrder(createOrderDTO);

        assertNotNull(result);
        assertEquals("PENDING", result.getStatus());
    }
}
