import { render, screen } from '@testing-library/react';
import { Welcome } from './Welcome';

describe('Welcome component', () => {
  it('getting started button has correct link', () => {
    render(<Welcome />);
    expect(screen.getByRole('button', { name: /Get Started/i })).toHaveAttribute(
      'href',
      'https://forresttech.github.io/exemplum/'
    );
  });

  it('github button has correct link', () => {
    render(<Welcome />);
    expect(screen.getByRole('button', { name: /Github/i })).toHaveAttribute(
      'href',
      'https://github.com/ForrestTech/Exemplum'
    );
  });
});
