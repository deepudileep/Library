// WarmupsTests.cs
using LibraryApis.Common;
using Xunit;

public class WarmupsTests
{
    [Fact]
    public void TestIsPowerOfTwo()
    {
        Assert.True(Warmups.IsPowerOfTwo(1));
        Assert.True(Warmups.IsPowerOfTwo(2));
        Assert.True(Warmups.IsPowerOfTwo(1024));
        Assert.False(Warmups.IsPowerOfTwo(0));
        Assert.False(Warmups.IsPowerOfTwo(3));
        Assert.False(Warmups.IsPowerOfTwo(-8));
    }

    [Fact]
    public void TestReverseTitle()
    {
        Assert.Equal("kciD yboM", Warmups.ReverseTitle("Moby Dick"));
    }

    [Fact]
    public void TestRepeatTitle()
    {
        Assert.Equal("ReadReadRead", Warmups.RepeatTitle("Read", 3));
        Assert.Equal("", Warmups.RepeatTitle("Read", 0));
    }

    [Fact]
    public void TestListOddIds()
    {
        var arr = Warmups.ListOddIdsUpTo100();
        Assert.Equal(50, arr.Length);
        Assert.Equal(1, arr[0]);
        Assert.Equal(99, arr[^1]);
    }
}
