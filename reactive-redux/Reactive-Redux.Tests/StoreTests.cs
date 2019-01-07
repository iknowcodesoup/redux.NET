
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Redux.Tests
{
  public static class StoreExtensions
  {
    public static void SubscribeAndGetState<TState>(this IStore<TState> store, Action<TState> listener)
    {
      store.StateChanged += () => listener(store.CurrentState);
    }
  }

  public class StoreTests
  {
    [Fact]
    public void Should_push_initial_state()
    {
      var sut = new Store<int>(Reducers.PassThrough, 1);
      var spyListener = new SpyListener<int>();

      sut.SubscribeAndGetState(spyListener.Listen);

      Assert.Equal(new[] { 1 }, spyListener.Values);
    }

    [Fact]
    public void Should_push_state_on_dispatch()
    {
      var sut = new Store<int>(Reducers.Replace, 1);
      var spyListener = new SpyListener<int>();

      sut.SubscribeAndGetState(spyListener.Listen);
      sut.Dispatch(new FakeAction<int>(2));

      Assert.Equal(new[] { 1, 2 }, spyListener.Values);
    }

    [Fact]
    public void Should_only_push_the_last_state_before_subscription()
    {
      var sut = new Store<int>(Reducers.Replace, 1);
      var spyListener = new SpyListener<int>();

      sut.Dispatch(new FakeAction<int>(2));
      sut.SubscribeAndGetState(spyListener.Listen);

      Assert.Equal(new[] { 2 }, spyListener.Values);
    }

    [Fact]
    public void Middleware_should_be_called_for_each_action_dispatched()
    {
      var numberOfCalls = 0;
      Middleware<int> spyMiddleware = store => next => action =>
      {
        numberOfCalls++;
        return next(action);
      };

      var sut = new Store<int>(Reducers.Replace, 1, spyMiddleware);
      var spyListener = new SpyListener<int>();

      sut.SubscribeAndGetState(spyListener.Listen);
      sut.Dispatch(new FakeAction<int>(2));

      Assert.Equal(1, numberOfCalls);
      Assert.Equal(new[] { 1, 2 }, spyListener.Values);
    }

    //[Fact]
    //public void Should_push_state_to_end_of_queue_on_nested_dispatch()
    //{
    //  var sut = new Store<int>(Reducers.Replace, 1);
    //  var spyListener = new SpyListener<int>();
    //  sut.SubscribeAndGetState(val =>
    //  {
    //    if (val < 5)
    //    {
    //      sut.Dispatch(new FakeAction<int>(val + 1));
    //    }
    //    spyListener.Listen(val);
    //  });

    //  Assert.Equal(new[] { 1, 2, 3, 4, 5 }, spyListener.Values);
    //}

    [Fact]
    public void CurrentState_should_return_initial_state()
    {
      var sut = new Store<int>(Reducers.Replace, 1);

      Assert.Equal(1, sut.CurrentState);
    }

    [Fact]
    public void CurrentState_should_return_the_latest_state()
    {
      var sut = new Store<int>(Reducers.Replace, 1);

      sut.Dispatch(new FakeAction<int>(2));

      Assert.Equal(2, sut.CurrentState);
    }

    [Fact]
    public async Task Store_should_be_thread_safe()
    {
      var sut = new Store<int>((state, action) => state + 1, 0);

      await Task.WhenAll(Enumerable.Range(0, 1000)
          .Select(_ => Task.Factory.StartNew(() => sut.Dispatch(new FakeAction<int>(0)))));

      Assert.Equal(1000, sut.CurrentState);
    }
  }
}