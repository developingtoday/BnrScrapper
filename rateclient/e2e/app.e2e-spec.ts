import { RateclientPage } from './app.po';

describe('rateclient App', function() {
  let page: RateclientPage;

  beforeEach(() => {
    page = new RateclientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
