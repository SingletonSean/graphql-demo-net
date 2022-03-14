using FluentValidation;
using GraphQLDemo.API.Schema.Mutations;

namespace GraphQLDemo.API.Validators
{
    public class InstructorTypeInputValidator : AbstractValidator<InstructorTypeInput>
    {
        public InstructorTypeInputValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty();
            RuleFor(i => i.LastName).NotEmpty();
        }
    }
}
