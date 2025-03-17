package csc13001.plantpos.application.services;

import java.util.Date;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import csc13001.plantpos.adapters.repositories.DiscountProgramRepository;
import csc13001.plantpos.domain.models.DiscountProgram;
import csc13001.plantpos.exception.discount.DiscountException;

@Service
public class DiscountProgramService {
    @Autowired
    private DiscountProgramRepository discountProgramRepository;

    public List<DiscountProgram> getDiscountPrograms() {
        return discountProgramRepository.findAll();
    }

    public DiscountProgram createDiscountProgram(DiscountProgram discountProgram) {
        Date startDate = discountProgram.getStartDate();
        Date endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }
        return discountProgramRepository.save(discountProgram);
    }

    public DiscountProgram getDiscountProgramById(Long id) {
        try {
            return discountProgramRepository.findById(id).get();
        } catch (Exception e) {
            throw new DiscountException.DiscountNotFoundException();
        }
    }

    public void updateDiscountProgram(Long id, DiscountProgram discountProgram) {
        DiscountProgram existingDiscount = discountProgramRepository.findById(id)
                .orElseThrow(() -> new DiscountException.DiscountNotFoundException());

        Date startDate = discountProgram.getStartDate();
        Date endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }

        discountProgramRepository.save(existingDiscount);
    }

    public void deleteDiscountProgram(Long id) {
        try {
            discountProgramRepository.deleteById(id);
        } catch (Exception e) {
            throw new DiscountException.DiscountNotFoundException();
        }
    }
}
