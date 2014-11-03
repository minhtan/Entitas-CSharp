﻿namespace Entitas {
    public class ReactiveEntityWillBeRemovedSystem : IEntitySystem {
        public IReactiveSubEntityWillBeRemovedSystem subsystem { get { return _subsystem; } }

        readonly IReactiveSubEntityWillBeRemovedSystem _subsystem;
        readonly EntityWillBeRemovedEntityRepositoryObserver _observer;

        public ReactiveEntityWillBeRemovedSystem(EntityRepository repo, IReactiveSubEntityWillBeRemovedSystem subSystem) {
            _subsystem = subSystem;
            _observer = new EntityWillBeRemovedEntityRepositoryObserver(repo, subSystem.GetTriggeringIndex());
        }

        public void Execute() {
            var buffer = new EntityComponentPair[_observer.collectedEntityComponentPairs.Count];
            _observer.collectedEntityComponentPairs.CopyTo(buffer, 0);
            _observer.ClearCollectedEntites();
            if (buffer.Length > 0) {
                _subsystem.Execute(buffer);
            }
        }
    }
}
