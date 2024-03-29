﻿using System;
using System.Collections.Generic;
using System.Data;
using DDD.Domain.Entities;
using DDD.Domain.Repositories;
using DDD.WinForm.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDDTest.Tests
{
    [TestClass]
    public class WeatherLatestViewModelTest
    {
        [TestMethod]
        public void シナリオ()
        {

            var weatherMock = new Mock<IWeatherRepository>();
            weatherMock.Setup(x => x.GetLatest(1)).Returns(
                new WeatherEntity(1, Convert.ToDateTime("2019/01/01 12:34:56"), 2, 12.3f));

            weatherMock.Setup(x => x.GetLatest(2)).Returns(
                new WeatherEntity(2, Convert.ToDateTime("2019/02/02 12:34:56"), 1, 22.4f));

            var areas = new List<AreaEntity>();
            areas.Add(new AreaEntity(1, "東京"));
            areas.Add(new AreaEntity(2, "神戸"));
            areas.Add(new AreaEntity(3, "沖縄"));
            var areasMock = new Mock<IAreaRepository>();
            areasMock.Setup(x => x.GetData()).Returns(areas);

            var viewModel = new WeatherLatestViewModel(weatherMock.Object, areasMock.Object);
            Assert.IsNull(viewModel.SelectedAreaId);
            Assert.AreEqual("", viewModel.DataDateText);
            Assert.AreEqual("", viewModel.ConditionText);
            Assert.AreEqual("", viewModel.TemperatureText);
            Assert.AreEqual(3, viewModel.Areas.Count);
            Assert.AreEqual(1, viewModel.Areas[0].AreaId);
            Assert.AreEqual("東京", viewModel.Areas[0].AreaName);

            viewModel.SelectedAreaId = 1;
            viewModel.Search();

            Assert.AreEqual(1, viewModel.SelectedAreaId);
            Assert.AreEqual("2019/01/01 12:34:56", viewModel.DataDateText);
            Assert.AreEqual("曇り", viewModel.ConditionText);
            Assert.AreEqual("12.30℃", viewModel.TemperatureText);

            viewModel.SelectedAreaId = 2;
            viewModel.Search();

            Assert.AreEqual(2, viewModel.SelectedAreaId);
            Assert.AreEqual("2019/02/02 12:34:56", viewModel.DataDateText);
            Assert.AreEqual("晴れ", viewModel.ConditionText);
            Assert.AreEqual("22.40℃", viewModel.TemperatureText);

            viewModel.SelectedAreaId = 3;
            viewModel.Search();

            Assert.AreEqual(3, viewModel.SelectedAreaId);
            Assert.AreEqual("", viewModel.DataDateText);
            Assert.AreEqual("", viewModel.ConditionText);
            Assert.AreEqual("", viewModel.TemperatureText);


        }
    }

}
