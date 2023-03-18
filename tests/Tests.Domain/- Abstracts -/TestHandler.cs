// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Jeebs.Data.Query;
using Jeebs.Logging;
using MaybeF.Caching;
using NSubstitute.Extensions;
using StrongId;

namespace Abstracts;

public abstract class TestHandler
{
	internal record class Vars<THandler>()
	{
		public IDispatcher Dispatcher { get; init; } =
			Substitute.For<IDispatcher>();

		public ILog<THandler> Log { get; init; } =
			Substitute.For<ILog<THandler>>();
	}

	internal record class Vars<TRepo, TEntity, TId, THandler> : Vars<THandler>
		where TRepo : class, IRepository<TEntity, TId>
		where TEntity : IWithId<TId>
		where TId : class, IStrongId, new()
	{
		public IMaybeCache<TId> Cache { get; init; } =
			Substitute.For<IMaybeCache<TId>>();

		public IFluentQuery<TEntity, TId> Fluent { get; init; } =
			Substitute.For<IFluentQuery<TEntity, TId>>();

		public TRepo Repo { get; init; } =
			Substitute.For<TRepo>();

		public Vars()
		{
			Fluent.ReturnsForAll(Fluent);
			Repo.StartFluentQuery().ReturnsForAll(Fluent);
		}
	}

	internal abstract class Setup<THandler> : Setup<THandler, Vars<THandler>> { }

	internal abstract class Setup<THandler, TVars>
		where TVars : Vars<THandler>, new()
	{
		internal abstract THandler GetHandler(TVars v);

		internal virtual (THandler handler, TVars v) GetVars()
		{
			// Create substitutes
			var dispatcher = Substitute.For<IDispatcher>();
			var log = Substitute.For<ILog<THandler>>();

			// Build handler
			var v = new TVars { Dispatcher = dispatcher, Log = log };
			var handler = GetHandler(v);

			// Return vars
			return (handler, v);
		}
	}

	internal abstract class Setup<TRepo, TEntity, TId, THandler> : Setup<TRepo, TEntity, TId, THandler, Vars<TRepo, TEntity, TId, THandler>>
		where TRepo : class, IRepository<TEntity, TId>
		where TEntity : IWithId<TId>
		where TId : class, IStrongId, new()
	{ }

	internal abstract class Setup<TRepo, TEntity, TId, THandler, TVars>
		where TRepo : class, IRepository<TEntity, TId>
		where TEntity : IWithId<TId>
		where TId : class, IStrongId, new()
		where TVars : Vars<TRepo, TEntity, TId, THandler>, new()
	{
		internal abstract THandler GetHandler(TVars v);

		internal virtual (THandler handler, TVars v) GetVars()
		{
			// Create substitutes
			var cache = Substitute.For<IMaybeCache<TId>>();
			var dispatcher = Substitute.For<IDispatcher>();
			var fluent = Substitute.For<IFluentQuery<TEntity, TId>>();
			var log = Substitute.For<ILog<THandler>>();
			var repo = Substitute.For<TRepo>();

			// Setup substitutes
			fluent.ReturnsForAll(fluent);
			repo.StartFluentQuery().Returns(fluent);

			// Build handler
			var v = new TVars { Cache = cache, Dispatcher = dispatcher, Fluent = fluent, Log = log, Repo = repo };
			var handler = GetHandler(v);

			// Return vars
			return (handler, v);
		}
	}
}
