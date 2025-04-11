package csc13001.plantpos.notification;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import csc13001.plantpos.notification.models.NotificationUser;

@Repository
public interface NotificationUserRepository extends JpaRepository<NotificationUser, Long> {
    List<NotificationUser> findByUser_UserId(Long userId);

    @Query("SELECT nu FROM NotificationUser nu JOIN FETCH nu.notification n JOIN FETCH nu.user")
    List<NotificationUser> findAllWithNotificationAndUser();
}
