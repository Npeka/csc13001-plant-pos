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
        return discountProgramRepository.findById(id)
                .orElseThrow(DiscountException.DiscountNotFoundException::new);
    }

    public void updateDiscountProgram(Long id, DiscountProgram discountProgram) {
        DiscountProgram existingDiscount = discountProgramRepository.findById(id)
                .orElseThrow(DiscountException.DiscountNotFoundException::new);

        Date startDate = discountProgram.getStartDate();
        Date endDate = discountProgram.getEndDate();
        if (startDate.compareTo(endDate) > 0) {
            throw new DiscountException.DiscountInvalidDateException();
        }

        discountProgramRepository.save(existingDiscount);
    }

    public void deleteDiscountProgram(Long id) {
        if (!discountProgramRepository.existsById(id)) {
            throw new DiscountException.DiscountNotFoundException();
        }
        discountProgramRepository.deleteById(id);
    }
}
