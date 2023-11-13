namespace EngineBay.DemoModule
{
    using FluentValidation;

    public class CreateTodoListDtoValidator : AbstractValidator<CreateOrUpdateTodoListDto>
    {
        public CreateTodoListDtoValidator()
        {
            this.RuleFor(createTodoListDto => createTodoListDto.Name).NotNull().NotEmpty();
        }
    }
}