using System;
using System.Collections.Generic;

namespace MVCSample.Infrastructure.DI
{
    public class DIInstallerSystem : IDIRegistrationSystem
    {
        private Dictionary<Type, Action<IDIRegistratorAPI>> _bindingsToActivate;

        private State _state;

        public DIInstallerSystem()
        {
            _bindingsToActivate = new();

            _state = State.Registration;
        }

        public void ActivateBindings(IDIRegistratorAPI dIRegistrator)
        {
            if (_state != State.Activation)
                return;

            foreach (Type type in _bindingsToActivate.Keys)
                _bindingsToActivate[type].Invoke(dIRegistrator);

            _state = State.Default;
        }

        public void StopRegistration()
        {
            if (_state != State.Registration)
                return;

            _state = State.Activation;
        }

        public HashSet<Type> GetContracts()
        {
            return new(_bindingsToActivate.Keys);
        }

        #region Registrator API

        public void Register<TInterface, TImplementation>(bool asSingle = true) where TImplementation : TInterface
        {
            Type type = CheckTypeRepeatCondition<TInterface>();
            _bindingsToActivate.Add(type, r => r.Register<TInterface, TImplementation>(asSingle));
        }

        public void RegisterInstance<TInterface>(TInterface instance)
        {
            Type type = CheckTypeRepeatCondition<TInterface>();
            _bindingsToActivate.Add(type, r => r.RegisterInstance(instance));
        }

        public void RegisterFromMethod<TInterface>(Func<TInterface> method)
        {
            Type type = CheckTypeRepeatCondition<TInterface>();
            _bindingsToActivate.Add(type, r => r.RegisterFromMethod(method));
        }

        private Type CheckTypeRepeatCondition<TInterface>()
        {
            if (_state != State.Registration)
                throw new Exception("0");

            Type type = typeof(TInterface);

            if (_bindingsToActivate.ContainsKey(type))
                throw new Exception("1");

            return type;
        }

        #endregion

        #region enum State

        public enum State
        {
            Registration,
            Activation,
            Default
        }

        #endregion
    }
}
