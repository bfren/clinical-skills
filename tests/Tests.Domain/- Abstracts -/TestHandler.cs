// Clinical Skills: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Jeebs.Cqrs;
using Jeebs.Data;
using Jeebs.Data.Query;
using Jeebs.Data.Testing.Query;
using Jeebs.Logging;
using MaybeF.Caching;
using StrongId;

namespace Abstracts;

internal abstract class TestHandlerBase<THandler>
{
	internal record class Vars
	{
		internal IDispatcher Dispatcher { get; init; }

		internal ILog<THandler> Log { get; init; }

		public Vars()
		{
			// Create substitutes
			Dispatcher = Substitute.For<IDispatcher>();
			Log = Substitute.For<ILog<THandler>>();
		}
	}

	internal abstract class SetupBase : SetupBase<Vars> { }

	internal abstract class SetupBase<TVars>
		where TVars : Vars, new()
	{
		internal abstract THandler GetHandler(TVars v);

		internal virtual (THandler handler, TVars v) GetVars()
		{
			// Build handler
			var v = new TVars();
			var handler = GetHandler(v);

			// Return vars
			return (handler, v);
		}
	}
}

internal abstract class TestHandlerBase<TRepo, TEntity, TId, THandler> : TestHandlerBase<THandler>
	where TRepo : class, IRepository<TEntity, TId>
	where TEntity : IWithId<TId>
	where TId : class, IStrongId, new()
{
	internal new record class Vars : TestHandlerBase<THandler>.Vars
	{
		internal IMaybeCache<TId> Cache { get; init; }

		internal IFluentQuery<TEntity, TId> Fluent { get; init; }

		internal TRepo Repo { get; init; }

		public Vars()
		{
			// Create substitutes
			Cache = Substitute.For<IMaybeCache<TId>>();
			Fluent = FluentQueryHelper.CreateSubstitute<TEntity, TId>();
			Repo = Substitute.For<TRepo>();

			// Setup substitutes
			Repo.StartFluentQuery().Returns(Fluent);
		}
	}

	internal new abstract class SetupBase : SetupBase<Vars> { }

	internal new abstract class SetupBase<TVars> : TestHandlerBase<THandler>.SetupBase<TVars>
		where TVars : Vars, new()
	{ }
}
