namespace EngineBay.DemoModule
{
    using FluentValidation;

    public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemDtoValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
            this.RuleFor(x => x.ListId).NotNull().NotEmpty();
        }
    }
}