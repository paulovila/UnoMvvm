using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnoMvvm
{
    /// <summary>
    /// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public sealed class AsyncCommand<T> : PropertyObserverBase, IAsyncCommand<T>
    {
        readonly Func<T, Task> _execute;
        Func<object?, bool> _canExecute;
        private readonly Action<Exception>? _onException;
        private readonly bool _continueOnCapturedContext;
        private readonly WeakEventManager _weakEventManager = new WeakEventManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1"/> class.
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public AsyncCommand(Func<T, Task> execute,
                            Func<object?, bool>? canExecute = null,
                            Action<Exception>? onException = null,
                            bool continueOnCapturedContext = false)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value);
            remove => _weakEventManager.RemoveEventHandler(value);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state
        /// </summary>
        /// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object? parameter) => _canExecute(parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public override void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public Task ExecuteAsync(T parameter) => _execute(parameter);

        void ICommand.Execute(object parameter)
        {
            switch (parameter)
            {
                case T validParameter:
                    ExecuteAsync(validParameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
                    break;

#pragma warning disable CS8601 //Possible null reference assignment
                case null when !typeof(T).GetTypeInfo().IsValueType:
                    // ReSharper disable once ExpressionIsAlwaysNull
                    ExecuteAsync((T)parameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
                    break;
#pragma warning restore CS8601

                case null:
                    throw new InvalidCommandParameterException(typeof(T));

                default:
                    throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
            }
        }
        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="TType">The type of the return value of the method that this delegate encapsulates</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public AsyncCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public AsyncCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
            _canExecute = p => expression.Compile().Invoke((T)p);
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }

    /// <summary>
    /// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public sealed class AsyncCommand : PropertyObserverBase, IAsyncCommand
    {
        private readonly Func<Task> _execute;
        private Func<bool> _canExecute;
        private readonly Action<Exception>? _onException;
        readonly bool _continueOnCapturedContext;
        private readonly WeakEventManager _weakEventManager = new WeakEventManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1"/> class.
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAysnc is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public AsyncCommand(Func<Task> execute,
                            Func<bool>? canExecute = null,
                            Action<Exception>? onException = null,
                            bool continueOnCapturedContext = false)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (() => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value);
            remove => _weakEventManager.RemoveEventHandler(value);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state
        /// </summary>
        /// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object? parameter) => _canExecute();

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public override void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        public Task ExecuteAsync() => _execute();

        void ICommand.Execute(object parameter) => _execute().SafeFireAndForget(_continueOnCapturedContext, _onException);

        /// <summary>
        /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
        /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public AsyncCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
        /// </summary>
        /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
        /// <returns>The current instance of DelegateCommand</returns>
        public AsyncCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecute = () => canExecuteExpression.Compile().Invoke();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }
    }
}
