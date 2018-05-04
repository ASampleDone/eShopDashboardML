﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using System.Threading.Tasks;

namespace eShopDashboard.Forecast
{
    /// <summary>
    /// This is the input to the trained model.
    /// </summary>
    public class CountryData
    {
        // next,country,year,month,max,min,idx,count,sales,avg,prev
        public CountryData(string country, int year, int month, float max, float min, float idx, int count, float sales, float avg, float prev)
        {
            this.country = country;

            this.year = year;
            this.month = month;
            this.max = max;
            this.min = min;
            this.idx = idx;
            this.count = count;
            this.sales = sales;
            this.avg = avg;
            this.prev = prev;
        }

        [ColumnName("Label")]
        public float next;

        public string country;

        public float year;
        public float month;
        public float max;
        public float min;
        public float idx;
        public float count;
        public float sales;
        public float avg;
        public float prev;
    }

    /// <summary>
    /// This is the output of the scored model, the prediction.
    /// </summary>
    public class CountrySalesPrediction
    {
        // Below columns are produced by the model's predictor.
        public float Score;
    }

    public class CountrySales : ICountrySales
    {
        /// <summary>
        /// This method demonstrates how to run prediction on one example at a time.
        /// </summary>
        public async Task<CountrySalesPrediction> Predict(string modelPath, string country, int year, int month, float max, float min, float idx, int count, float sales, float avg, float prev)
        {
            // Load model
            var predictionEngine = await CreatePredictionEngineAsync(modelPath);

            // Build country sample
            var countrySample = new CountryData(country, year, month, max, min, idx, count, sales, avg, prev);

            // Returns prediction
            return predictionEngine.Predict(countrySample);
        }

        /// <summary>
        /// This function creates a prediction engine from the model located in the <paramref name="modelPath"/>.
        /// </summary>
        private async Task<PredictionModel<CountryData, CountrySalesPrediction>> CreatePredictionEngineAsync(string modelPath)
        {
            var env = new TlcEnvironment(conc: 1);
            PredictionModel<CountryData, CountrySalesPrediction> model = await PredictionModel.ReadAsync<CountryData, CountrySalesPrediction>(modelPath);
            return model;
        }
    }
}
