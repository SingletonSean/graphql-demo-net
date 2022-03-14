using AppAny.HotChocolate.FluentValidation;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Validators;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class InstructorMutation
    {
        [Authorize]
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<InstructorResult> CreateInstructor(
            [UseFluentValidation, UseValidator<InstructorTypeInputValidator>] InstructorTypeInput instructorInput,
            [ScopedService] SchoolDbContext context,
            [Service] ITopicEventSender topicEventSender)
        {
            InstructorDTO instructorDTO = new InstructorDTO()
            {
                FirstName = instructorInput.FirstName,
                LastName = instructorInput.LastName,
                Salary = instructorInput.Salary,
            };

            context.Add(instructorDTO);
            await context.SaveChangesAsync();

            InstructorResult instructorResult = new InstructorResult()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };

            await topicEventSender.SendAsync(nameof(Subscription.InstructorCreated), instructorResult);

            return instructorResult;
        }

        [Authorize]
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<InstructorResult> UpdateInstructor(
            Guid id,
            [UseFluentValidation, UseValidator<InstructorTypeInputValidator>] InstructorTypeInput instructorInput,
            [ScopedService] SchoolDbContext context)
        {
            InstructorDTO instructorDTO = await context.Instructors.FindAsync(id);

            if(instructorDTO == null)
            {
                throw new GraphQLException(new Error("Instructor not found.", "INSTRUCTOR_NOT_FOUND"));
            }

            instructorDTO.FirstName = instructorInput.FirstName;
            instructorDTO.LastName = instructorInput.LastName;
            instructorDTO.Salary = instructorInput.Salary;

            context.Update(instructorDTO);
            await context.SaveChangesAsync();

            InstructorResult instructorResult = new InstructorResult()
            {
                Id = instructorDTO.Id,
                FirstName = instructorDTO.FirstName,
                LastName = instructorDTO.LastName,
                Salary = instructorDTO.Salary,
            };

            return instructorResult;
        }

        [Authorize(Policy = "IsAdmin")]
        [UseDbContext(typeof(SchoolDbContext))]
        public async Task<bool> DeleteInstructor(Guid id, [ScopedService] SchoolDbContext context)
        {
            InstructorDTO instructorDTO = new InstructorDTO()
            {
                Id = id
            };

            context.Remove(instructorDTO);

            try
            {
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
