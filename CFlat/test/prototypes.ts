import 'should'
export module prototype_extensions {
    ''.isBlank().should.equal(true);
    '  '.isBlank().should.equal(true);
    'NotBlank'.isBlank().should.equal(false);
    
    const array = [1, 2, 3];
    [].any().should.equal(false);
    array.any().should.equal(true);

    array.first().should.equal(1).and.be.a.Number();
    array.last().should.equal(3).and.be.a.Number();

    array.spliceAt(1).should.equal(2).and.be.a.Number();
    array.should.deepEqual([1, 3]).and.be.an.Array();

    array.push(1, 2);
    array.popElements(1, 2).should.equal(1).and.be.a.Number();
    array.should.deepEqual([3]).and.be.an.Array();
    
    console.log("All tests passed!");
}
