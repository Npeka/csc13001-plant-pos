package csc13001.plantpos.user;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import csc13001.plantpos.user.models.WorkLog;

@Repository
public interface WorkLogRepository extends JpaRepository<WorkLog, Long> {
}
