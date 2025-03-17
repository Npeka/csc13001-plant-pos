package csc13001.plantpos.application.services;

import csc13001.plantpos.adapters.repositories.StaffRepository;
import csc13001.plantpos.domain.models.Staff;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class StaffService {
    private final StaffRepository staffRepository;

    public StaffService(StaffRepository staffRepository) {
        this.staffRepository = staffRepository;
    }

    @Transactional
    public void updateSales(Long staffId) {
        Staff staff = staffRepository.findById(staffId).orElseThrow();
        staff.updateSales(100.0);
        staffRepository.save(staff);
    }
}
