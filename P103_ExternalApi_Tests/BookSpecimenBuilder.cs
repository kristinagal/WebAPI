//using AutoFixture.Kernel;
//using System;
//using System.Collections.Generic;
//using P103_ExternalApi.Dtos;
//using AutoFixture;

//namespace P103_ExternalApi_Tests
//{
//    internal class BookSpecimenBuilder : ISpecimenBuilder
//    {
//        public object Create(object request, ISpecimenContext context)
//        {
//            var type = request as Type;

//            if (type == typeof(BookApiResult))
//            {
//                return new BookApiResult
//                {
//                    Id = context.Create<int>(), // Use AutoFixture to create an int ID
//                    Title = $"Book_{context.Create<int>()}", // Consistent title generation
//                    Authors = context.Create<List<int>>(),
//                    Genres = new List<string> { "Fiction", "Adventure" },
//                    Year = context.Create<int>() % (DateTime.Now.Year - 1900) + 1900 // Create a year between 1900 and the current year
//                };
//            }

//            if (type == typeof(BookResult))
//            {
//                return new BookResult
//                {
//                    Id = context.Create<int>(),
//                    Title = $"Book_{context.Create<int>()}",
//                    Authors = context.Create<List<int>>(),
//                    Genres = new List<string> { "Fiction", "Adventure" },
//                    Year = context.Create<int>() % (DateTime.Now.Year - 1900) + 1900
//                };
//            }

//            return new NoSpecimen();
//        }
//    }
//}
