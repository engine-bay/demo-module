namespace EngineBay.DemoModule
{
    using FluentValidation;

    public class CreateTodoListDtoValidator : AbstractValidator<CreateTodoListDto>
    {
        public CreateTodoListDtoValidator()
        {
            this.RuleFor(createTodoListDto => createTodoListDto.Name).NotNull().NotEmpty();
        }
    }
}