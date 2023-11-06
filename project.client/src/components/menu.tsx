import { useState } from "react";
import { Link } from "react-router-dom";
import { Button, Collapse, Nav, NavItem, NavLink, Navbar, NavbarBrand, NavbarToggler } from 'reactstrap';
import { userActions } from "./reducers/usersSlice";
import { useAppDispatch, useAppSelector } from "./storage";

export function Menu() {
    const dispatch = useAppDispatch();
    const user = useAppSelector(x => x.userState.user);
    const [collapsed, setCollapsed] = useState(false);

    if (!user) {
        return <></>;
    }

    return (
        <header>
            <Navbar color="dark" expand="md" container="fluid" fixed="top" className="border-bottom">
                <NavbarBrand tag={Link} to="/">
                    <img alt="logo" src="/ufo.svg" style={{ height: 40, width: 40 }} />
                </NavbarBrand>
                <NavbarToggler onClick={() => setCollapsed(!collapsed)} />
                <Collapse isOpen={collapsed} navbar>
                    <Nav className="me-auto" navbar>
                        <NavItem>
                            <NavLink tag={Link} to="/stack-tags">Fetch tags</NavLink>
                        </NavItem>
                    </Nav>
                    <Button outline color="primary" onClick={() => dispatch(userActions.logout(user))}>
                        {'< Take me away'}
                    </Button>
                </Collapse>
            </Navbar>
        </header>
    );
}