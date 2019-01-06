
using System;
using Xunit;

namespace Redux.Tests
{
  public class StoreExtensionsTests
  {
    [Fact]
    public void ObserveState_should_push_store_states()
    {
      var sut = new Store<int>(Reducers.Replace, 1);
      var spyListener = new SpyListener<int>();

      sut.ObserveState().Subscribe(spyListener.Listen);
      sut.Dispatch(new FakeAction<int>(2));

      Assert.Equal(new[] { 1, 2 }, spyListener.Values);
    }
  }
}
