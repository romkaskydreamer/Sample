﻿using System;
using System.Collections.Generic;
using Sample.Infrastructure.EventSourcing.Events;

namespace Sample.Infrastructure.EventSourcing.Aggregates
{
    public abstract class Aggregate : IAggregate
    {
        public const int MaxShardKey = 10000;
        private static readonly Random _shardKeyGenerator = new Random();
        private readonly IList<IAggregateEvent> _changes = new List<IAggregateEvent>();
        

        protected Aggregate(Guid id)
            : this(id, GenerateShardKey())
        {
        }

        protected Aggregate(Guid id, int shardKey)
        {
            Id = id;
            Version = 0;
            ShardKey = shardKey;
        }


        // TODO: Remove it and make proper deserialization
        protected Aggregate(Guid id, int version, int shardKey)
        {
            Id = id;
            Version = version;
            ShardKey = shardKey;
        }


        public Guid Id { get; private set; }

        public int Version { get; private set; }

        public int ShardKey { get; private set; }


        public IList<IAggregateEvent> GetUncommitedChanges()
        {
            return this._changes;
        }

        public void MarkAsCommited()
        {
            this._changes.Clear();
        }


        public void ApplyEvent(IAggregateEvent @event)
        {
            if (this.Id != @event.AggregateId)
                return;

            this.Version = @event.Version;
            this.ShardKey = @event.ShardKey;
            this.CallApply(@event);

            this._changes.Add(@event);
        }

        protected void CallApply(IAggregateEvent @event)
        {
            ((dynamic) this).Apply((dynamic) @event);
        }


        private static int GenerateShardKey()
        {
            return _shardKeyGenerator.Next(0, MaxShardKey);
        }
    }
}
