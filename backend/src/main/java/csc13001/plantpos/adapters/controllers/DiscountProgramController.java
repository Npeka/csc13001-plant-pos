package csc13001.plantpos.adapters.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import csc13001.plantpos.application.services.DiscountProgramService;
import csc13001.plantpos.domain.models.DiscountProgram;
import csc13001.plantpos.utils.http.HttpResponse;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@RestController
@RequestMapping("/api/discounts")
public class DiscountProgramController {
    @Autowired
    private DiscountProgramService discountProgramService;

    @GetMapping
    public ResponseEntity<?> getDiscountPrograms() {
        List<DiscountProgram> discountPrograms = discountProgramService.getDiscountPrograms();
        return HttpResponse.ok("get discount programs successfully", discountPrograms);
    }
}
