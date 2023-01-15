﻿using System.Diagnostics;
using AutoMapper;
using SimpleCalculator.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SimpleCalculator.Web.Models;
using SimpleCalculator.DataAccess.Data;

namespace SimpleCalculator.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICalculationService<CalculationInputModel> _service;
    private readonly CalculatorDbContext _context;

    public HomeController(ICalculationService<CalculationInputModel> service, CalculatorDbContext context)
    {
        _service = service;
        _context = context;
    }

    public IActionResult Index()
    {
        var e = _context.CalculationResultEntities.ToList();

        var pModel = new CalculationPageModel()
        {
            Input = new CalculationInputModel(),
            Result = new List<CalculationEntityModel>(),
        };

        
        foreach (var entity in e)
        {
            var entities = new List<CalculationEntityModel>
        {
            new CalculationEntityModel
            {
                Id = entity.Id,
                MathOperator = entity.MathOperator,
                FirstNumber = entity.FirstNumber,
                SecondNumber = entity.SecondNumber,
                Result = entity.Result,
            }
        };
        }

        return View(pModel);
    }

    [HttpPost]
    public IActionResult Add(CalculationInputModel model)
    {
        _service.Add(model);
        _service.AddToDb(model);
        _service.Save();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult Subtract(CalculationInputModel model)
    {
        _service.Subtract(model);
        _service.AddToDb(model);
        _service.Save();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult Multiply(CalculationInputModel model)
    {
        _service.Multiply(model);
        _service.AddToDb(model);
        _service.Save();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult Divide(CalculationInputModel model)
    {
        if (model.SecondNumber == 0)
        {
            ModelState.AddModelError("SecondNumber", "Unable to divide by zero.");
        }
        else
        {
            _service.Divide(model);
            _service.AddToDb(model);
            _service.Save();
        }
        return View("Index", model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
