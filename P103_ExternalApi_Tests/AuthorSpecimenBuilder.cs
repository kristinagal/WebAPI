//using AutoFixture.Kernel;
//using P103_ExternalApi.Dtos;
//using AutoFixture;
//using System;

//namespace P103_ExternalApi.Tests
//{
//    public class AuthorSpecimenBuilder : ISpecimenBuilder
//    {
//        public object Create(object request, ISpecimenContext context)
//        {
//            // Check if the request is for a type we want to customize
//            if (request is Type type && type == typeof(AuthorApiResult))
//            {
//                return new AuthorApiResult
//                {
//                    Id = context.Create<int>(),
//                    Name = $"Author_{context.Create<int>()}"
//                };
//            }

//            if (request is Type requestType && requestType == typeof(AuthorRequest))
//            {
//                return new AuthorRequest
//                {
//                    Name = $"AuthorRequest_{context.Create<int>()}"
//                };
//            }

//            // If not a recognized type, return NoSpecimen to allow AutoFixture's default behavior
//            return new NoSpecimen();
//        }
//    }
//}
