const rhino3dm = require('rhino3dm')

let rhino
beforeEach( async() => {
    rhino = await rhino3dm()
  })
//TODO
// Skipping for now.
test('DeleteDimStyle', async () => {

  const file3dm = new rhino.File3dm()
  file3dm.applicationName = 'rhino3dm.js'
  file3dm.applicationDetails = 'rhino3dm-tests-dimStyleTable-deleteDimStyle'
  file3dm.applicationUrl = 'https://rhino3d.com'

  const ds1 = new rhino.DimensionStyle()
  const ds2 = new rhino.DimensionStyle()

  // .bitmaps().add() is void
  model.dimstyles().add(ds1)
  model.dimstyles().add(ds2)

  const qtyDimStyles1 = model.dimstyles().count

  const id1 = model.dimstyles().get(0).id

  model.dimstyles().delete(id1)

  const qtyDimStyles2 = model.dimstyles().count

  expect(qtyDimStyles1 === 2 && qtyDimStyles2 === 1).toBe(true)

})