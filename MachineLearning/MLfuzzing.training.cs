﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace CShidori
{
    public partial class MLfuzzing
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Text.FeaturizeText(@"uuid", @"uuid")      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(@"response", @"response"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(@"request", @"request"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"uuid",@"response",@"request"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(@"vulnerable", @"vulnerable"))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(l1Regularization:0.143282010149996F,l2Regularization:1.29943317699872F,labelColumnName:@"vulnerable",featureColumnName:@"Features"))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(@"PredictedLabel", @"PredictedLabel"));

            return pipeline;
        }
    }
}