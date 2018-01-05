import 'should'
export module prototype_extensions {
    ''.isBlank().should.equal(true);
    '  '.isBlank().should.equal(true);
    'NotBlank'.isBlank().should.equal(false);
    
    const array = [1, 2, 3];
    array.first().should.equal(1).and.be.a.Number();
    array.last().should.equal(3).and.be.a.Number();
    
    console.log("All tests passed!");
}
