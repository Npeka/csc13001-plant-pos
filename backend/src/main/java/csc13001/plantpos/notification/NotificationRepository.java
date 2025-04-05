package csc13001.plantpos.notification;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import csc13001.plantpos.notification.models.Notification;

@Repository
public interface NotificationRepository extends JpaRepository<Notification, Long> {
}
