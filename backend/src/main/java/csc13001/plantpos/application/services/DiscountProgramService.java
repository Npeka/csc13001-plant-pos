package csc13001.plantpos.application.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import csc13001.plantpos.adapters.repositories.DiscountProgramRepository;
import csc13001.plantpos.domain.models.DiscountProgram;

@Service
public class DiscountProgramService {
    @Autowired
    private DiscountProgramRepository discountProgramRepository;

    public List<DiscountProgram> getDiscountPrograms() {
        return discountProgramRepository.findAll();
    }
}
