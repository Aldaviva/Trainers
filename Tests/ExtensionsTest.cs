#nullable enable

using FluentAssertions;
using TrainerCommon;

namespace Tests;

public class ExtensionsTest {

    [Fact]
    public void headValueType() {
        IEnumerable<int> enumerable = new[] { 1, 2, 3 };
        enumerable.HeadAndTailStruct().head.Should().Be(1);
    }

    [Fact]
    public void headReferenceType() {
        IEnumerable<string> enumerable = new[] { "1", "2", "3" };
        enumerable.HeadAndTail().head.Should().Be("1");
    }

    [Fact]
    public void emptyHeadValueType() {
        IEnumerable<int> enumerable = Array.Empty<int>();
        enumerable.HeadAndTailStruct().head.Should().BeNull();
    }

    [Fact]
    public void emptyHeadReferenceType() {
        IEnumerable<string> enumerable = Array.Empty<string>();
        enumerable.HeadAndTail().head.Should().BeNull();
    }

    [Fact]
    public void tailValueType() {
        IEnumerable<int> enumerable = new[] { 1, 2, 3 };
        enumerable.HeadAndTailStruct().tail.Should().Equal(2, 3);
    }

    [Fact]
    public void tailReferenceType() {
        IEnumerable<string> enumerable = new[] { "1", "2", "3" };
        enumerable.HeadAndTail().tail.Should().Equal("2", "3");
    }

    [Fact]
    public void noTailValueTypes() {
        IEnumerable<int> enumerable = new[] { 1 };
        enumerable.HeadAndTailStruct().tail.Should().BeEmpty();
    }

    [Fact]
    public void noTailReferenceTypes() {
        IEnumerable<string> enumerable = new[] { "1" };
        enumerable.HeadAndTail().tail.Should().BeEmpty();
    }

    [Fact]
    public void valueTypes() {
        new[] { true }.HeadAndTailStruct().head.Should().Be(true);
        new[] { (byte) 0 }.HeadAndTailStruct().head.Should().Be(0);
        new[] { (sbyte) 0 }.HeadAndTailStruct().head.Should().Be(0);
        new[] { (short) 0 }.HeadAndTailStruct().head.Should().Be(0);
        new[] { (ushort) 0 }.HeadAndTailStruct().head.Should().Be(0);
        new[] { 0u }.HeadAndTailStruct().head.Should().Be(0);
        new[] { 0L }.HeadAndTailStruct().head.Should().Be(0);
        new[] { 0UL }.HeadAndTailStruct().head.Should().Be(0);
        new[] { IntPtr.Zero }.HeadAndTailStruct().head.Should().Be(IntPtr.Zero);
        new[] { UIntPtr.Zero }.HeadAndTailStruct().head.Should().Be(UIntPtr.Zero);
        new[] { 'a' }.HeadAndTailStruct().head.Should().Be('a');
        new[] { 0.0 }.HeadAndTailStruct().head.Should().Be(0.0);
        new[] { 0.0f }.HeadAndTailStruct().head.Should().Be(0.0f);
        new[] { DateTime.Today }.HeadAndTailStruct().head.Should().Be(DateTime.Today);
        new[] { MyEnum.A }.HeadAndTailStruct().head.Should().Be(MyEnum.A);
    }

    [Fact]
    public void list() {
        IEnumerable<string> enumerable = new List<string> { "1", "2", "3" };
        (string? head, IEnumerable<string> tail) = enumerable.HeadAndTail();
        head.Should().Be("1");
        tail.Should().Equal("2", "3");
    }

    [Fact]
    public void range() {
        IEnumerable<int> enumerable = Enumerable.Range(1, 100);
        (int? head, IEnumerable<int> tail) = enumerable.HeadAndTailStruct();
        head.Should().Be(1);
        tail.Should().HaveCount(99);
    }

    [Fact]
    public void nullableValueTypes() {
        IEnumerable<int?> enumerable = new int?[] { 1, 2, 3 };
        (int? head, IEnumerable<int?>? tail) = enumerable.HeadAndTailStruct();
        head.Should().Be(1);
        tail.Should().Equal(2, 3);
    }

    [Fact]
    public void nullableReferenceTypes() {
        IEnumerable<string?> enumerable = new[] { "1", "2", "3" };
        (string? head, IEnumerable<string?>? tail) = enumerable.HeadAndTail();
        head.Should().Be("1");
        tail.Should().Equal("2", "3");
    }

    private enum MyEnum {

        A,
        B,
        C

    }

}